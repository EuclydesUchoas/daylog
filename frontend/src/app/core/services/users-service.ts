import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ApiResponse, PagedResult } from '../models/api-response';
import { User } from '../models/user';
import { map, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class UsersService {
  private readonly baseUrl = 'https://localhost:7176/api/v1/users';

  constructor(private http: HttpClient) {}

  getUsers(): Observable<User[]> {
    return this.http.get<ApiResponse<PagedResult<User>>>(this.baseUrl).pipe(
      map(response => {
        if (response.isFailure) {
          throw new Error(response.error?.description || 'Erro ao buscar usuários');
        }
        return response.data?.items ?? [];
      })
    );
  }
}
