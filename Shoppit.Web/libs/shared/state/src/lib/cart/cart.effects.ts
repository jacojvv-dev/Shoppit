import { Injectable } from '@angular/core';
import { createEffect, Actions, ofType, OnInitEffects } from '@ngrx/effects';
import { Action } from '@ngrx/store';
import { fetch } from '@nrwl/angular';
import { CartService } from '@shoppit/shared/services';
import { map, switchMap } from 'rxjs/operators';

import * as CartActions from './cart.actions';
import * as CartFeature from './cart.reducer';

@Injectable()
export class CartEffects implements OnInitEffects {
  init$ = createEffect(() =>
    this.actions$.pipe(
      ofType(CartActions.init),
      fetch({
        run: (action) => {
          return this.cartService
            .getCartItems()
            .pipe(
              switchMap((response) => [
                CartActions.loadCartSuccess({ cart: response }),
                CartActions.loadCartSummary(),
              ])
            );
        },
        onError: (action, error) => {
          console.error('Error', error);
          return CartActions.loadCartFailure({ error });
        },
      })
    )
  );

  addOrUpdateCartItem$ = createEffect(() =>
    this.actions$.pipe(
      ofType(CartActions.addOrUpdateCartItem),
      fetch({
        run: (action) => {
          return this.cartService
            .addOrUpdateCartItem(action.request)
            .pipe(
              switchMap((response) => [
                CartActions.addOrUpdateCartItemSuccess({ item: response }),
                CartActions.loadCartSummary(),
              ])
            );
        },
        onError: (action, error) => {
          console.error('Error', error);
          return CartActions.addOrUpdateCartItemFailure({ error });
        },
      })
    )
  );

  removeCartItem$ = createEffect(() =>
    this.actions$.pipe(
      ofType(CartActions.removeCartItem),
      fetch({
        run: (action) => {
          return this.cartService
            .removeCartItem(action.id)
            .pipe(
              switchMap((response) => [
                CartActions.removeCartItemSuccess({ id: action.id }),
                CartActions.loadCartSummary(),
              ])
            );
        },
        onError: (action, error) => {
          console.error('Error', error);
          return CartActions.removeCartItemFailure({ error });
        },
      })
    )
  );

  getCartSummary$ = createEffect(() =>
    this.actions$.pipe(
      ofType(CartActions.loadCartSummary),
      fetch({
        run: (_) => {
          return this.cartService
            .getCartSummary()
            .pipe(
              map((response) =>
                CartActions.loadCartSummarySuccess({ summary: response })
              )
            );
        },
        onError: (action, error) => {
          console.error('Error', error);
          return CartActions.loadCartSummaryFailure({ error });
        },
      })
    )
  );

  checkoutCart$ = createEffect(() =>
    this.actions$.pipe(
      ofType(CartActions.checkoutCart),
      fetch({
        run: (_) => {
          console.log('xxx');
          return this.cartService
            .checkout()
            .pipe(map((_) => CartActions.init()));
        },
        onError: (action, error) => {
          console.error('Error', error);
          return CartActions.checkoutCartFailure({ error });
        },
      })
    )
  );

  ngrxOnInitEffects(): Action {
    return CartActions.init();
  }

  constructor(
    private readonly actions$: Actions,
    private cartService: CartService
  ) {}
}
