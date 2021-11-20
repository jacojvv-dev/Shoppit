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
  constructor(private productsFacade: ProductsFacade) {
    this.products$ = this.productsFacade.allProducts$;
  }

  ngOnInit(): void {}
}
