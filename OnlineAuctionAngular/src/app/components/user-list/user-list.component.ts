import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/models/user.model';
import { UserService } from 'src/app/services/user.service';
import { RouterModule } from '@angular/router';
import { AlertService } from 'src/app/services/alert.service';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
})
export class UserListComponent implements OnInit {

  limit: number = 10;
  offset: number = 0;
  page: number = 1;
  users: User[];
  totalCount: number;

  constructor(private userService: UserService, 
    private router: RouterModule,
    private alertService: AlertService) { }

  ngOnInit() {
    this.getUsers();
  }

  getUsers(){
    this.userService.getAll(this.limit, this.offset).subscribe(
      (usersList) => {         
        if (!usersList){
          this.alertService.error("Users not found.");
          this.users = null;
          return;
        }          
        this.users = usersList.users;
        this.totalCount = usersList.totalCount;
      },
      (e) => console.log(e));
  }

  onPageChanged(page: number){
    window.scroll(0,0);
    this.offset = (this.limit * page) - this.limit;
    this.getUsers();
  }

  changeLimit(limit: number){
    window.scroll(0,0);
    this.limit = limit;
    this.offset = 0;
    this.page = 1;
    this.getUsers();
  }

  deleteUser(userId: number){
    this.userService.delete(userId).subscribe(() => {
      this.users = this.users.filter(user => user.userProfileId !== userId);
      this.alertService.success("User deleted successfully.");
      window.scroll(0,0);
    },
    (err) => this.alertService.error(err.error.error));
  }
}
