import { createAction, props } from '@ngrx/store';
import { PaginatedResponse, ProductResponse } from '@shoppit/shared/types';

export const init = createAction('[Products Page] Init');

export const loadProductsSuccess = createAction(
  '[Products/API] Load Products Success',
  props<{ response: PaginatedResponse<ProductResponse> }>()
);

export const loadProductsFailure = createAction(
  '[Products/API] Load Products Failure',
  props<{ error: any }>()
);

export const setSelectedProduct = createAction(
  '[Products/API] Set Selected Product',
  props<{ id: string }>()
);

export const loadProductDetail = createAction(
  '[Products/API] Load Product Detail',
  props<{ id: string }>()
);

export const loadProductDetailSuccess = createAction(
  '[Products/API] Load Product Detail Success',
  props<{ product: ProductResponse }>()
);

export const loadProductDetailFailure = createAction(
  '[Products/API] Load Product Detail Failure',
  props<{ error: any }>()
);

export const nextPage = createAction('[Products Page] Next Page');
export const previousPage = createAction('[Products Page] Previous Page');
export const search = createAction(
  '[Products Page] Search',
  props<{ query: string }>()
);
