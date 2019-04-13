import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/services/user.service';
import { RouterModule, ActivatedRoute } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';
import { AlertService } from 'src/app/services/alert.service';
import { User } from 'src/app/models/user.model';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent implements OnInit {
  public user: User;
  
  constructor(private userService: UserService, 
    private authService: AuthService, 
    private route: ActivatedRoute,
    private router: RouterModule,
    private alertService: AlertService) { }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.userService.get(+params['id']).subscribe(user => this.user = user);
    });
  }  
  
  public get isUserAdmin$(): Observable<boolean> {
    return this.authService.isAdmin();
  }

  public get isUserOwner$(): Observable<boolean> {
    return this.authService.getCurrentUser().pipe(
      map(user => user.userProfileId == this.user.userProfileId)
    );
  }
}
