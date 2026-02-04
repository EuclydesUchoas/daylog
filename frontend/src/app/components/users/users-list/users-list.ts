import { ChangeDetectorRef, Component, computed, DestroyRef, effect, inject, OnInit, signal } from '@angular/core';
import { User } from '../../../core/models/user';
import { UsersService } from '../../../core/services/users-service';
import { MessageService } from 'primeng/api';
import { TableModule } from 'primeng/table';
import { ToastModule } from 'primeng/toast';
import { CommonModule } from '@angular/common';
import { ApiResponse, PagedResult } from '../../../core/models/api-response';
import { BehaviorSubject, catchError, finalize, Observable, of, Subject, takeUntil, takeWhile } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

@Component({
  selector: 'app-users-list',
  imports: [CommonModule, TableModule, ToastModule],
  templateUrl: './users-list.html',
  styleUrl: './users-list.scss',
  providers: []
})
export class UsersList implements OnInit {
  users = signal<User[]>([]);
  // mainSubject?: Subject<User[]>;
  destroyRef = inject(DestroyRef);

  users$!: Observable<User[]>;

  loading = signal(false);

  messageService = inject(MessageService);

  constructor(
    private usersService: UsersService,
    // private messageService: MessageService
  ) {}

  ngOnInit(): void {
    // this.loadUsers();
    this.loadUsers2();
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

  loadUsers2() {
    this.loading.set(true);
    this.usersService.getUsers().pipe(takeUntilDestroyed(this.destroyRef)).subscribe({
    // this.usersService.getUsers().pipe(takeWhile(mainSubject)).subscribe({
      next: (users) => {
        this.users.set(users);
        this.loading.set(false);
      },
      error: (err) => {
        // this.messageService.add({
        //   severity: 'error',
        //   summary: 'Erro',
        //   detail: err.error.error?.description || err.message
        // });
        this.loading.set(false);
      }
    });
  }
}
