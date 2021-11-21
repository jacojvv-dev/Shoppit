import { Injectable } from '@angular/core';
import { select, Store, Action } from '@ngrx/store';
import { AddOrUpdateCartItemRequest } from '@shoppit/shared/types';

import * as CartActions from './cart.actions';
import * as CartFeature from './cart.reducer';
import * as CartSelectors from './cart.selectors';

@Injectable()
export class CartFacade {
  loaded$ = this.store.pipe(select(CartSelectors.getCartLoaded));
  allCart$ = this.store.pipe(select(CartSelectors.getAllCart));
  selectedCart$ = this.store.pipe(select(CartSelectors.getSelected));
  totalCartItems$ = this.store.pipe(select(CartSelectors.getCountOfItems));
  cartSummary$ = this.store.pipe(select(CartSelectors.getCartSummary));

  constructor(private readonly store: Store) {}

  addToCart(request: AddOrUpdateCartItemRequest) {
    this.store.dispatch(CartActions.addOrUpdateCartItem({ request }));
  }

  removeCartItem(id: string) {
    this.store.dispatch(CartActions.removeCartItem({ id }));
  }

  getCart() {
    this.store.dispatch(CartActions.init());
  }
}
