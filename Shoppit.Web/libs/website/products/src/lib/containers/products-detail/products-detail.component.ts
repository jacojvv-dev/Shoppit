import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ProductResponse } from '@shoppit/shared/types';
import { ProductsFacade } from '@shoppit/website/products';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Component({
  selector: 'shoppit-products-detail',
  templateUrl: './products-detail.component.html',
  styleUrls: ['./products-detail.component.css'],
})
export class ProductsDetailComponent implements OnInit {
  selectedProduct$: Observable<ProductResponse | undefined>;
  constructor(
    private productsFacade: ProductsFacade,
    private route: ActivatedRoute
  ) {
    this.selectedProduct$ = productsFacade.selectedProducts$;
  }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.productsFacade.setSelectedProductId(id);
    }
  }
}
