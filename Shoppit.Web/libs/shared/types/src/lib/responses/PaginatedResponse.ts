export type PaginatedResponse<T> = {
  items: T[];
  totalItems: number;
  totalPages: number;
  page: number;
  nextPage?: number;
  previousPage?: number;
};
