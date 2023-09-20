
import { Component, OnInit } from '@angular/core';
import { UserService } from '../../services/user.service';
import { ActivatedRoute, Router } from '@angular/router';
import { User } from '../../models/user.model';

@Component({
  selector: 'app-user-edit',
  templateUrl: './user-edit.component.html',
  styleUrls: ['./user-edit.component.css']
})
export class UserEditComponent implements OnInit {
  user: User | undefined;
  editedUser: User = { id: 0, firstName: '', lastName: '', email: '' };

  constructor(
    private userService: UserService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    const userId = Number(this.route.snapshot.paramMap.get('id'));

    if (!isNaN(userId)) {
      this.userService.getUser(userId).subscribe((data) => {
        this.user = data;

        if (this.user) {
          this.editedUser = { ...this.user };
        }
      });
    }
  }

  updateUser(): void {
    if (this.user) {
      this.userService.updateUser(this.user.id, this.editedUser).subscribe(() => {
        this.router.navigate(['/users', this.user?.id]);
      });
    }
  }

  cancelEdit(): void {
    if (this.user) {
      this.router.navigate(['/users', this.user.id]);
    }
  }
}
