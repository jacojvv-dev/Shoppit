import { Injectable } from '@angular/core';
import { select, Store, Action } from '@ngrx/store';

import * as ProductsActions from './products.actions';
import * as ProductsFeature from './products.reducer';
import * as ProductsSelectors from './products.selectors';

@Injectable()
export class ProductsFacade {
  /**
   * Combine pieces of state using createSelector,
   * and expose them as observables through the facade.
   */
  loaded$ = this.store.pipe(select(ProductsSelectors.getProductsLoaded));
  allProducts$ = this.store.pipe(select(ProductsSelectors.getAllProducts));
  selectedProducts$ = this.store.pipe(select(ProductsSelectors.getSelected));
  nextPage$ = this.store.pipe(select(ProductsSelectors.getNextPage));
  previousPage$ = this.store.pipe(select(ProductsSelectors.getPreviousPage));

  constructor(private readonly store: Store) {}

  setSelectedProductId(id: string) {
    this.store.dispatch(ProductsActions.setSelectedProduct({ id }));
    this.store.dispatch(ProductsActions.loadProductDetail({ id }));
  }

  getNextPage() {
    this.store.dispatch(ProductsActions.nextPage());
  }

  getPreviousPage() {
    this.store.dispatch(ProductsActions.previousPage());
  }

  searchProducts(query: string) {
    this.store.dispatch(ProductsActions.search({ query }));
  }
}
