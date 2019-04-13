import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { User } from 'src/app/models/user.model';

@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.css']
})
export class NavigationComponent implements OnInit {
  currentUser: User;

  constructor(private authService: AuthService, private router: Router) { }

  public get isUserSignedIn$(): Observable<boolean> {
    return this.authService.isSignedIn();
  }

  public get isUserAdmin$(): Observable<boolean> {    
    return this.authService.isAdmin();
  }

  public get isUserSeller$(): Observable<boolean> {    
    return this.authService.isSeller();
  }
    
  public signOut(): void {
    this.authService.signOut().subscribe(() => {
      this.router.navigate([''])
    })
  }
  ngOnInit() {
    this.authService.getCurrentUser().subscribe(
      user => this.currentUser = user
    )
  }

}
