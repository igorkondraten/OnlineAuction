import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';
import { UserLoginModel } from 'src/app/models/user-login-model';

@Component({
  selector: 'app-sign-in',
  templateUrl: './sign-in.component.html',
  styleUrls: ['./sign-in.component.css']
})
export class SignInComponent implements OnInit {

  username: string;
  password: string;

  constructor(private router: Router, private authService: AuthService) { }

  login(): void {
    this.authService.signIn(<UserLoginModel>{userName: this.username, password: this.password})
    .subscribe(() => {
     this.router.navigate(['']);
    }, (e) => {
      console.log(e);
      if (e.status == 400)
        alert("Username or password is invalid.");
      if (e.status == 500)
        alert("Server error");
    });
  }

  ngOnInit() {
  }

}
