import { ProductResponse } from '.';

export type CartItemResponse = {
  id: string;
  quantity: number;
  product: ProductResponse;
};
