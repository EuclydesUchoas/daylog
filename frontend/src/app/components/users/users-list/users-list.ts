import { ChangeDetectorRef, Component, computed, effect, OnInit, signal } from '@angular/core';
import { User } from '../../../core/models/user';
import { UsersService } from '../../../core/services/users-service';
import { MessageService } from 'primeng/api';
import { TableModule } from 'primeng/table';
import { ToastModule } from 'primeng/toast';
import { CommonModule } from '@angular/common';
import { ApiResponse, PagedResult } from '../../../core/models/api-response';
import { catchError, finalize, Observable, of } from 'rxjs';

@Component({
  selector: 'app-users-list',
  imports: [CommonModule, TableModule, ToastModule],
  templateUrl: './users-list.html',
  styleUrl: './users-list.scss',
  providers: [MessageService]
})
export class UsersList implements OnInit {
  // users = signal<User[]>([]);
  users$!: Observable<User[]>;
  loading = signal(false);

  constructor(
    private usersService: UsersService,
    private messageService: MessageService
  ) {}

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers() {
    this.loading.set(true);
    this.users$ = this.usersService.getUsers()
      .pipe(
        catchError(err => {
          this.messageService.add({
            severity: 'error',
            summary: 'Erro',
            detail: err.error.error?.description || err.message
          });
          return of([]);
        }),
        finalize(() => this.loading.set(false))
      );
  }
}
