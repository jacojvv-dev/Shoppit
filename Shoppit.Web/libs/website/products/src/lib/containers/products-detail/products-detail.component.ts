import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ProductResponse } from '@shoppit/shared/types';
import { ProductsFacade } from '@shoppit/website/products';
import { OAuthService } from 'angular-oauth2-oidc';
import { Observable, Subscription } from 'rxjs';

import { CartFacade } from '@shoppit/shared/state';

@Component({
  selector: 'shoppit-products-detail',
  templateUrl: './products-detail.component.html',
  styleUrls: ['./products-detail.component.css'],
})
export class ProductsDetailComponent implements OnInit, OnDestroy {
  selectedProduct$: Observable<ProductResponse | undefined>;
  authSvcEventSubscription$: Subscription | undefined;
  isLoggedIn = false;
  quantity = 1;
  id: string | null = null;

  constructor(
    private productsFacade: ProductsFacade,
    private cartFacade: CartFacade,
    private route: ActivatedRoute,
    private authSvc: OAuthService
  ) {
    this.selectedProduct$ = productsFacade.selectedProducts$;
    this.isLoggedIn = authSvc.hasValidAccessToken();
  }

  ngOnInit(): void {
    this.authSvcEventSubscription$ = this.authSvc.events.subscribe(
      (ev) => (this.isLoggedIn = this.authSvc.hasValidAccessToken())
    );

    this.id = this.route.snapshot.paramMap.get('id');
    if (this.id) this.productsFacade.setSelectedProductId(this.id);
  }

  addToCart() {
    const { id, quantity } = this;

    this.cartFacade.addToCart({
      productId: id as string,
      quantity: quantity,
    });
  }

  ngOnDestroy(): void {
    this.authSvcEventSubscription$?.unsubscribe();
  }
}
