import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  constructor(private authService: AuthService, private router: Router) { }

  public get isUserSignedIn$(): Observable<boolean> {
    return this.authService.isSignedIn();
  }
  
  public signOut(): void {
    this.authService.signOut().subscribe(() => {
      this.router.navigate([''])
    })
  }
  ngOnInit() {
  }

}
