import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { UserRegisterModel } from 'src/app/models/user-register-model';
import { UserLoginModel } from 'src/app/models/user-login-model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.css']
})
export class SignUpComponent {

  public userName: string;
  public password: string;
  public email: string;

  constructor(private authService: AuthService, private router: Router) { }

  public signUp(): void {
    this.authService.signUp(<UserRegisterModel> { Name: this.userName, password: this.password, email: this.email})
    .subscribe((user) => {
      console.log("Returned to component");
      this.authService.signIn(<UserLoginModel> { userName: this.userName, password: this.password}).subscribe(
        _ => { this.router.navigate(['']) })
    });

    // this.authService.validateLogin(this.login).pipe(
    //   tap(isValid => {
    //     if (!isValid) {
    //       alert('user with such login already exists');
    //     }
    //   }),
    //   filter(isValid => isValid),
    //   mergeMap(_ =>
    //     this.authService.singUp(<UserLoginModel> { login: this.login, password: this.password },
    //       this.firstName, this.lastName))
    // ).subscribe(_ => {
    //   this.router.navigate(['dashboards'])
    // });
  }

  ngOnInit() {
  }

}
