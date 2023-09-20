import { Component, OnInit } from '@angular/core';
import { UserService } from '../../services/user.service';
import { User } from '../../models/user.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-user-create',
  templateUrl: './user-create.component.html',
  styleUrls: ['./user-create.component.css']
})
export class UserCreateComponent implements OnInit {
  firstName: string = '';
  lastName: string = '';
  email: string = '';

  constructor(
    private userService: UserService,
    private router: Router
  ) {}

  ngOnInit(): void {}

  createUser(): void {
    const newUser: User = {
      id: 0,
      firstName: this.firstName,
      lastName: this.lastName,
      email: this.email
    };

    this.userService.createUser(newUser).subscribe((createdUser) => {
      this.router.navigate(['/users', createdUser.id]);
    });
  }

  goBackToList(): void {
    this.router.navigate(['/users']);
  }
}
