import { Component, OnInit, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { UsersList } from './components/users/users-list/users-list';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, UsersList],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  protected readonly title = signal('daylog-webapp');

  show = signal(true);

  constructor() {
    setTimeout(() => {
      // this.show.set(false);
      this.show.set(true);
    }, 4000);
  }
}
