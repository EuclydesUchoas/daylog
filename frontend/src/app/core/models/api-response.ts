export interface ApiResponse<T> {
  isSuccess: boolean;
  isFailure: boolean;
  error?: ApiError;
  data?: T;
}

export interface ApiError {
  code: string;
  description: string;
  type: number;
}

export interface PagedResult<T> {
  items: T[];
}
