import { createEntityAdapter, EntityAdapter, EntityState } from '@ngrx/entity';
import { Action, createReducer, on } from '@ngrx/store';
import { CartItemResponse, CartSummaryResponse } from '@shoppit/shared/types';
import * as CartActions from './cart.actions';

export const CART_FEATURE_KEY = 'cart';

export interface State extends EntityState<CartItemResponse> {
  selectedId?: string | number; // which Cart record has been selected
  summary: CartSummaryResponse | undefined;
  loaded: boolean; // has the Cart list been loaded
  error?: string | null; // last known error (if any)
}

export interface CartPartialState {
  readonly [CART_FEATURE_KEY]: State;
}

export const cartAdapter: EntityAdapter<CartItemResponse> =
  createEntityAdapter<CartItemResponse>();

export const initialState: State = cartAdapter.getInitialState({
  // set initial required properties
  loaded: false,
  summary: undefined,
});

const cartReducer = createReducer(
  initialState,
  on(CartActions.init, (state) => ({ ...state, loaded: false, error: null })),
  on(CartActions.loadCartSuccess, (state, { cart }) =>
    cartAdapter.setAll(cart, { ...state, loaded: true })
  ),
  on(CartActions.loadCartFailure, (state, { error }) => ({ ...state, error })),
  on(CartActions.addOrUpdateCartItemSuccess, (state, { item }) =>
    cartAdapter.upsertOne(item, { ...state })
  ),
  on(CartActions.addOrUpdateCartItemFailure, (state, { error }) => ({
    ...state,
    error,
  })),
  on(CartActions.removeCartItemSuccess, (state, { id }) =>
    cartAdapter.removeOne(id, { ...state })
  ),
  on(CartActions.removeCartItemFailure, (state, { error }) => ({
    ...state,
    error,
  })),
  on(CartActions.loadCartSummarySuccess, (state, { summary }) => ({
    ...state,
    summary,
  })),
  on(CartActions.loadCartSummaryFailure, (state, { error }) => ({
    ...state,
    error,
  })),
  on(CartActions.checkoutCart, (state) => ({
    ...state,
    loaded: false,
  })),
  on(CartActions.checkoutCartFailure, (state, { error }) => ({
    ...state,
    error,
  }))
);

export function reducer(state: State | undefined, action: Action) {
  return cartReducer(state, action);
}
