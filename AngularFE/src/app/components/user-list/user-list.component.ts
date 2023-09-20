import { Component, OnInit } from '@angular/core';
import { UserService } from '../../services/user.service';
import { User } from '../../models/user.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
})
export class UserListComponent implements OnInit {
  users: User[] = [];
  ascending = true;
  filterLastName = '';

  constructor(
    private userService: UserService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers(): void {
    this.userService.getUsers(this.ascending, this.filterLastName).subscribe((data) => {
      this.users = data;
    });
  }

  toggleSorting(): void {
    this.ascending = !this.ascending;
    this.loadUsers();
  }

  applyFilter(): void {
    this.loadUsers();
  }

  viewUserDetails(userId: number): void {
    this.router.navigate(['/users', userId]);
  }

  editUser(userId: number): void {
    this.router.navigate(['/edit', userId]);
  }

  createUser(): void {
    this.router.navigate(['/create']);
  }

  deleteUser(userId: number): void {
    this.userService.deleteUser(userId).subscribe(() => {
      this.users = this.users.filter(u => u.id !== userId);
    });
  }
}
