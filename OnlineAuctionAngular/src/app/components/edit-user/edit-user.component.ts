import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { AlertService } from 'src/app/services/alert.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { User } from 'src/app/models/user.model';
import { AuthService } from 'src/app/services/auth.service';
import { UserService } from 'src/app/services/user.service';
import { UserAddress } from 'src/app/models/user-address.model';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-edit-user',
  templateUrl: './edit-user.component.html',
  styleUrls: ['./edit-user.component.css']
})
export class EditUserComponent implements OnInit {
  user: User;
  editUserForm: FormGroup;
  loading = false;
  submitted = false;
  roles: string[] = ["Admin", "Seller", "User"];

  get f() { return this.editUserForm.controls; }

  constructor(private route: ActivatedRoute,
    private userService: UserService,
    private authService: AuthService,
    private router: Router,
    private alertService: AlertService,
    private formBuilder: FormBuilder) { }


  ngOnInit() {
    this.route.params.subscribe(params => {
      this.userService.get(+params['id']).subscribe(user => {
        this.user = user;
        this.updateValues();
      });
    });
    this.editUserForm = this.formBuilder.group({
      userName: ['', [Validators.required, Validators.maxLength(20)]],
      email: ['', [Validators.required, Validators.maxLength(100), Validators.email]],
      role: ['', Validators.required],
      country: ['', Validators.maxLength(100)],
      city: ['', Validators.maxLength(50)],
      street: ['', Validators.maxLength(200)],
      zipcode: ['', Validators.maxLength(18)]
    });
  }

  public get isUserAdmin$(): Observable<boolean> {
    return this.authService.isAdmin();
  }

  onSubmit() {
    this.submitted = true;
    if (this.editUserForm.invalid) {
      return;
    }
    this.loading = true;
    this.userService.update(this.user.userProfileId, <User>{
      name: this.f.userName.value,
      email: this.f.email.value,
      role: this.f.role.value,
      address: <UserAddress>{
        country: this.f.country.value,
        city: this.f.city.value,
        street: this.f.street.value,
        zipCode: this.f.zipcode.value
    }})
      .subscribe(
        () => {
          this.alertService.success('User updated');
          this.router.navigate(['/user', this.user.userProfileId]);
        },
        error => {
          this.alertService.error(error.error.message);
          this.loading = false;
        });
  }

  updateValues() {
    this.editUserForm.patchValue({
      userName: this.user.name,
      email: this.user.email,
      role: this.user.role,
      country: this.user.address.country,
      city: this.user.address.city,
      street: this.user.address.street,
      zipcode: this.user.address.zipCode
    });
  }
}
