import { Injectable } from '@angular/core';
import {
  createEffect,
  Actions,
  ofType,
  OnInitEffects,
  act,
} from '@ngrx/effects';
import { Action, Store } from '@ngrx/store';
import { fetch } from '@nrwl/angular';
import { ProductService } from '@shoppit/shared/services';
import { map, tap, withLatestFrom } from 'rxjs/operators';

import * as ProductsActions from './products.actions';
import * as ProductsFeature from './products.reducer';

@Injectable()
export class ProductsEffects implements OnInitEffects {
  init$ = createEffect(() =>
    this.actions$.pipe(
      ofType(ProductsActions.init),
      fetch({
        run: (action) => {
          return this.productService
            .listProducts()
            .pipe(
              map((response) =>
                ProductsActions.loadProductsSuccess({ products: response })
              )
            );
        },
        onError: (action, error) => {
          console.error('Error', error);
          return ProductsActions.loadProductsFailure({ error });
        },
      })
    )
  );

  loadProductDetail$ = createEffect(() =>
    this.actions$.pipe(
      ofType(ProductsActions.loadProductDetail),
      withLatestFrom(this.store),
      fetch({
        run: (action, state: any) => {
          const {
            ['website-products']: { entities },
          } = state;
          // if we already have the metadata we don't need to get the details again
          if (entities[action.id]?.metadata)
            return ProductsActions.loadProductDetailSuccess({
              product: entities[action.id],
            });

          return this.productService
            .loadProductDetails(action.id)
            .pipe(
              map((response) =>
                ProductsActions.loadProductDetailSuccess({ product: response })
              )
            );
        },
        onError: (action, error) => {
          console.error('Error', error);
          return ProductsActions.loadProductDetailFailure({ error });
        },
      })
    )
  );

  ngrxOnInitEffects(): Action {
    return ProductsActions.init();
  }

  constructor(
    private readonly actions$: Actions,
    private productService: ProductService,
    private store: Store
  ) {}
}
