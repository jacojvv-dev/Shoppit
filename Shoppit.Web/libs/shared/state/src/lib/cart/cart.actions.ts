import { createAction, props } from '@ngrx/store';
import {
  AddOrUpdateCartItemRequest,
  CartItemResponse,
  CartSummaryResponse,
} from '@shoppit/shared/types';

export const init = createAction('[Cart Page] Init');

export const loadCartSuccess = createAction(
  '[Cart/API] Load Cart Success',
  props<{ cart: CartItemResponse[] }>()
);

export const loadCartFailure = createAction(
  '[Cart/API] Load Cart Failure',
  props<{ error: any }>()
);

export const addOrUpdateCartItem = createAction(
  '[Cart/API] Add Or Update Cart Item',
  props<{ request: AddOrUpdateCartItemRequest }>()
);

export const addOrUpdateCartItemSuccess = createAction(
  '[Cart/API] Add Or Update Cart Item Success',
  props<{ item: CartItemResponse }>()
);

export const addOrUpdateCartItemFailure = createAction(
  '[Cart/API] Add Or Update Cart Item Failure',
  props<{ error: any }>()
);

export const removeCartItem = createAction(
  '[Cart/API] Remove Cart Item',
  props<{ id: string }>()
);

export const removeCartItemSuccess = createAction(
  '[Cart/API] Remove Cart Item Item Success',
  props<{ id: string }>()
);

export const removeCartItemFailure = createAction(
  '[Cart/API] Remove Cart Item Failure',
  props<{ error: any }>()
);

export const loadCartSummary = createAction('[Cart/API] Get Cart Summary');

export const loadCartSummarySuccess = createAction(
  '[Cart/API] Get Cart Summary Success',
  props<{ summary: CartSummaryResponse }>()
);

export const loadCartSummaryFailure = createAction(
  '[Cart/API] Get Cart Summary Failure',
  props<{ error: any }>()
);

export const checkoutCart = createAction('[Cart Page] Checkout Cart');

export const checkoutCartFailure = createAction(
  '[Cart/API] Checkout Cart Failure',
  props<{ error: any }>()
);
