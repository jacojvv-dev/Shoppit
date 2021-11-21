import { Component, OnInit } from '@angular/core';
import { CartFacade } from '@shoppit/shared/state';
import { CartItemResponse, CartSummaryResponse } from '@shoppit/shared/types';
import { Observable } from 'rxjs';
import { NgbModal, ModalDismissReasons } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'shoppit-cart-index',
  templateUrl: './cart-index.component.html',
  styleUrls: ['./cart-index.component.css'],
})
export class CartIndexComponent implements OnInit {
  cartItems$: Observable<CartItemResponse[]>;
  loaded$: Observable<boolean>;
  summary$: Observable<CartSummaryResponse | undefined>;
  selectedCartItemId: string = '';
  quantity: number = 0;

  constructor(private cartFacade: CartFacade, private modalService: NgbModal) {
    this.loaded$ = cartFacade.loaded$;
    this.cartItems$ = cartFacade.allCart$;
    this.summary$ = cartFacade.cartSummary$;
  }

  ngOnInit(): void {}

  onUpdateClicked(id: string, quantity: number, content: any) {
    this.selectedCartItemId = id;
    this.quantity = quantity;
    this.open(content);
  }

  onRemoveClicked(id: string, content: any) {
    this.selectedCartItemId = id;
    this.open(content);
  }

  private open(content: any) {
    this.modalService
      .open(content, { ariaLabelledBy: 'modal-basic-title' })
      .result.then(
        (result) => {
          if (result === 'Remove From Cart')
            this.cartFacade.removeCartItem(this.selectedCartItemId);
          if (result === 'Update Quantity')
            this.cartFacade.addToCart({
              productId: this.selectedCartItemId,
              quantity: this.quantity,
            });
        },
        (_) => {}
      );
  }
}
