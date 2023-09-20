import { Component, OnInit } from '@angular/core';
import { UserService } from '../../services/user.service';
import { User } from '../../models/user.model';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-user-details',
  templateUrl: './user-details.component.html',
  styleUrls: ['./user-details.component.css']
})
export class UserDetailsComponent implements OnInit {
  user: User | undefined;

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
      });
    }
  }

  editUser(): void {
    if (this.user) {
      this.router.navigate(['/edit', this.user.id]);
    }
  }

  goBackToList(): void {
    this.router.navigate(['/users']);
  }
}
