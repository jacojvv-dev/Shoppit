import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { APP_CONFIG } from '@shoppit/shared/app-config';
import { ProductResponse } from '@shoppit/shared/types';
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

  // page: string = '1', perPage: string = '25', query: string = ''
  listProducts() {
    // let params = {
    //   page,
    //   perPage,
    //   query,
    // };

    // if (page == null) {
    //   params['page'] = '1';
    // // }

    // {
    //   params,
    // }

    return this.httpClient.get<ProductResponse[]>(`${this.base}/api/Products`);
  }

  loadProductDetails(id: string) {
    return this.httpClient.get<ProductResponse>(
      `${this.base}/api/Products/${id}`
    );
  }
}
