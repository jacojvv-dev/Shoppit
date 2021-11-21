import { ProductImageResponse } from '.';
import { ProductMetadataResponse } from './ProductMetadataResponse';

export type ProductResponse = {
  id: string;
  name: string;
  description: string;
  price: number;
  images: ProductImageResponse[];
  metadata: ProductMetadataResponse[];
};
