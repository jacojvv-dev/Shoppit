import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { APP_CONFIG } from '@shoppit/shared/app-config';
import { PaginatedResponse, ProductResponse } from '@shoppit/shared/types';
@Injectable({
  providedIn: 'root',
})
export class ProductService {
  private base: string;
  constructor(
    @Inject(APP_CONFIG) private configuration: any,
    private httpClient: HttpClient
  ) {
    this.base = configuration.apiRoute;
  }

  listProducts(
    page: string = '1',
    perPage: string = '25',
    searchQuery?: string
  ) {
    let params: Record<string, string> = { page, perPage };
    if (searchQuery) params = { ...params, searchQuery: searchQuery as string };

    return this.httpClient.get<PaginatedResponse<ProductResponse>>(
      `${this.base}/api/Products`,
      {
        params,
      }
    );
  }

  loadProductDetails(id: string) {
    return this.httpClient.get<ProductResponse>(
      `${this.base}/api/Products/${id}`
    );
  }
}
