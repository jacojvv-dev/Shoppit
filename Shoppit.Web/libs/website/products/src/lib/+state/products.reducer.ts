import { createEntityAdapter, EntityAdapter, EntityState } from '@ngrx/entity';
import { Action, createReducer, on } from '@ngrx/store';
import { ProductResponse } from '@shoppit/shared/types';
import * as ProductsActions from './products.actions';

export const PRODUCTS_FEATURE_KEY = 'website-products';

export interface State extends EntityState<ProductResponse> {
  selectedId?: string | number;
  loaded: boolean;
  error?: string | null;
}

export interface ProductsPartialState {
  readonly [PRODUCTS_FEATURE_KEY]: State;
}

export const productsAdapter: EntityAdapter<ProductResponse> =
  createEntityAdapter<ProductResponse>();

export const initialState: State = productsAdapter.getInitialState({
  // set initial required properties
  loaded: false,
});

const productsReducer = createReducer(
  initialState,
  on(ProductsActions.init, (state) => ({
    ...state,
    loaded: false,
    error: null,
  })),
  on(ProductsActions.loadProductsSuccess, (state, { products }) =>
    productsAdapter.setAll(products, { ...state, loaded: true })
  ),
  on(ProductsActions.loadProductsFailure, (state, { error }) => ({
    ...state,
    error,
  })),
  on(ProductsActions.setSelectedProduct, (state, { id }) => ({
    ...state,
    selectedId: id,
  })),
  on(ProductsActions.loadProductDetailSuccess, (state, { product }) =>
    productsAdapter.upsertOne(product, { ...state })
  ),
  on(ProductsActions.loadProductDetailFailure, (state, { error }) => ({
    ...state,
    error,
  }))
);

export function reducer(state: State | undefined, action: Action) {
  return productsReducer(state, action);
}
