import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import * as fromProducts from './+state/products.reducer';
import { ProductsEffects } from './+state/products.effects';
import { ProductsFacade } from './+state/products.facade';
import { RouterModule } from '@angular/router';
import { ProductsIndexComponent } from './containers/products-index/products-index.component';
import { ProductsDetailComponent } from './containers/products-detail/products-detail.component';

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forChild([
      { path: '', pathMatch: 'full', component: ProductsIndexComponent },
      { path: ':id', component: ProductsDetailComponent },
    ]),
    StoreModule.forFeature(
      fromProducts.PRODUCTS_FEATURE_KEY,
      fromProducts.reducer
    ),
    EffectsModule.forFeature([ProductsEffects]),
  ],
  providers: [ProductsFacade],
  declarations: [ProductsIndexComponent, ProductsDetailComponent],
})
export class WebsiteProductsModule {}
