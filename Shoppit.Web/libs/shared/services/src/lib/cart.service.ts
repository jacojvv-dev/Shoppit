import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { APP_CONFIG } from '@shoppit/shared/app-config';
import {
  AddOrUpdateCartItemRequest,
  CartItemResponse,
  CartSummaryResponse,
} from '@shoppit/shared/types';
@Injectable({
  providedIn: 'root',
})
export class CartService {
  private base: string;
  constructor(
    @Inject(APP_CONFIG) private configuration: any,
    private httpClient: HttpClient
  ) {
    this.base = configuration.apiRoute;
  }

  getCartItems() {
    return this.httpClient.get<CartItemResponse[]>(`${this.base}/api/Cart`);
  }

  addOrUpdateCartItem(request: AddOrUpdateCartItemRequest) {
    return this.httpClient.post<CartItemResponse>(
      `${this.base}/api/Cart`,
      request
    );
  }

  getCartSummary() {
    return this.httpClient.get<CartSummaryResponse>(
      `${this.base}/api/Cart/Summary`
    );
  }

  removeCartItem(id: string) {
    return this.httpClient.delete(`${this.base}/api/Cart/${id}`);
  }

  checkout() {
    return this.httpClient.post(`${this.base}/api/Cart/Checkout`, {});
  }
}
