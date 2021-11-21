import { Component, OnInit } from '@angular/core';
import { ProductResponse } from '@shoppit/shared/types';
import { ProductsFacade } from '@shoppit/website/products';
import { Observable } from 'rxjs';

@Component({
  selector: 'shoppit-products-index',
  templateUrl: './products-index.component.html',
  styleUrls: ['./products-index.component.css'],
})
export class ProductsIndexComponent implements OnInit {
  products$: Observable<ProductResponse[]>;
  nextPage$: Observable<number | undefined>;
  previousPage$: Observable<number | undefined>;
  loaded$: Observable<boolean>;

  searchTerm: string = '';

  constructor(private productsFacade: ProductsFacade) {
    this.products$ = this.productsFacade.allProducts$;
    this.nextPage$ = this.productsFacade.nextPage$;
    this.previousPage$ = this.productsFacade.previousPage$;
    this.loaded$ = this.productsFacade.loaded$;
  }

  ngOnInit(): void {}

  onNextPageClicked() {
    this.productsFacade.getNextPage();
  }

  onPreviousPageClicked() {
    this.productsFacade.getPreviousPage();
  }

  search() {
    this.productsFacade.searchProducts(this.searchTerm);
  }
}
