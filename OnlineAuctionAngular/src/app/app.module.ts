import { BrowserModule } from '@angular/platform-browser';
import { NgModule, ModuleWithProviders, APP_INITIALIZER } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { SignUpComponent } from './components/sign-up/sign-up.component';
import { SignInComponent } from './components/sign-in/sign-in.component';
import { HttpClientModule, HTTP_INTERCEPTORS, HttpClient } from '@angular/common/http';
import { CurrentUserService } from './services/current-user.service';
import { AuthService } from './services/auth.service';
import { TokenInterceptor } from './interceptors/token.interceptor';
import { NavigationComponent } from './components/navigation/navigation.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {NgbModule, NgbActiveModal} from '@ng-bootstrap/ng-bootstrap';
import { LotComponent } from './components/lot/lot.component';
import { LotListComponent } from './components/lot-list/lot-list.component';
import { UserListComponent } from './components/user-list/user-list.component';
import { AlertComponent } from './components/alert/alert.component';
import { AddLotComponent } from './components/add-lot/add-lot.component';
import { EditLotComponent } from './components/edit-lot/edit-lot.component';
import { OwlDateTimeModule, OwlNativeDateTimeModule, OWL_DATE_TIME_FORMATS, OWL_DATE_TIME_LOCALE } from 'ng-pick-datetime';
import { UserComponent } from './components/user/user.component';
import { EditUserComponent } from './components/edit-user/edit-user.component';

@NgModule({
  declarations: [
    AppComponent,
    SignUpComponent,
    SignInComponent,
    NavigationComponent,
    LotComponent,
    LotListComponent,
    UserListComponent,
    AlertComponent,
    AddLotComponent,
    EditLotComponent,
    UserComponent,
    EditUserComponent
  ],
  imports: [
    HttpClientModule,
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    NgbModule,
    BrowserAnimationsModule,
    ReactiveFormsModule,
    OwlDateTimeModule, 
    OwlNativeDateTimeModule
    ],
  providers: [
    { provide: Window, useValue: window },
    { provide: OWL_DATE_TIME_LOCALE, useValue: 'en'},
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptor,
      multi: true
   },
   { 
     provide: APP_INITIALIZER, 
     useFactory: loadCurrentUser, 
     deps: [CurrentUserService], 
     multi: true 
   } ,
   AuthService,
   NgbActiveModal
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

function loadCurrentUser(currentUserService: CurrentUserService): () => Promise<boolean> {
  return () => currentUserService.loadCurrentUser();
}