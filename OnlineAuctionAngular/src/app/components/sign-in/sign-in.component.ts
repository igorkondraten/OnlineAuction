import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';
import { UserLoginModel } from 'src/app/models/user-login-model';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AlertService } from 'src/app/services/alert.service';

@Component({
  selector: 'app-sign-in',
  templateUrl: './sign-in.component.html',
  styleUrls: ['./sign-in.component.css']
})
export class SignInComponent implements OnInit {
  loginForm: FormGroup;
  loading = false;
  submitted = false;

  constructor(private router: Router,
    private authService: AuthService,
    private formBuilder: FormBuilder,
    private alertService: AlertService) { }

  onSubmit() {
    this.submitted = true;
    if (this.loginForm.invalid) {
      return;
    }
    this.loading = true;
    this.authService.signIn(<UserLoginModel>{ userName: this.f.username.value, password: this.f.password.value })
      .subscribe(() => {
        this.router.navigate(['']);
      }, (e) => {
        this.alertService.error("Invalid login or password.");
        this.loading = false;
      });
  }

  get f() { return this.loginForm.controls; }

  ngOnInit() {
    this.loginForm = this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

}
