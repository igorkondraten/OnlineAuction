import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { UserRegisterModel } from 'src/app/models/user-register-model';
import { UserLoginModel } from 'src/app/models/user-login-model';
import { Router } from '@angular/router';
import { AlertService } from 'src/app/services/alert.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { UserAddress } from 'src/app/models/user-address.model';

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.css']
})
export class SignUpComponent {
  registerForm: FormGroup;
  loading = false;
  submitted = false;

  constructor(private authService: AuthService,
    private router: Router,
    private alertService: AlertService,
    private formBuilder: FormBuilder) {
    this.authService.isSignedIn().subscribe(
      (isSignedIn) => {
        if (isSignedIn) {
          this.router.navigate(['/']);
        }
      }
    );
  }

  get f() { return this.registerForm.controls; }

  onSubmit() {
    this.submitted = true;
    if (this.registerForm.invalid) {
      return;
    }
    this.loading = true;
    this.authService.signUp(<UserRegisterModel>{
      name: this.f.userName.value,
      password: this.f.password.value,
      email: this.f.email.value,
      address: <UserAddress>{
        country: this.f.country.value,
        city: this.f.city.value,
        street: this.f.street.value,
        zipCode: this.f.zipcode.value
      }
    })
      .subscribe(
        () => {
          this.alertService.success('Registration successful');
          this.authService.signIn(<UserLoginModel>{ userName: this.f.userName.value, password: this.f.password.value }).subscribe(
            () => this.router.navigate(['']))
        },
        error => {
          this.alertService.error(error.error.message);
          this.loading = false;
        });
  }

  ngOnInit() {
    this.registerForm = this.formBuilder.group({
      userName: ['', [Validators.required, Validators.maxLength(20)]],
      email: ['', [Validators.required, Validators.maxLength(100), Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(100)]],
      country: ['', Validators.maxLength(100)],
      city: ['', Validators.maxLength(50)],
      street: ['', Validators.maxLength(200)],
      zipcode: ['', Validators.maxLength(18)]
    });
  }

}
