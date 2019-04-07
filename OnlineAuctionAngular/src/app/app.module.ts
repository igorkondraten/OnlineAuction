import { BrowserModule } from '@angular/platform-browser';
import { NgModule, ModuleWithProviders, APP_INITIALIZER } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { SignUpComponent } from './components/sign-up/sign-up.component';
import { SignInComponent } from './components/sign-in/sign-in.component';
import { HttpClientModule, HTTP_INTERCEPTORS, HttpClient } from '@angular/common/http';
import { CurrentUserService } from './services/current-user.service';
import { AuthService } from './services/auth.service';
import { TokenInterceptor } from './interceptors/token.interceptor';
import { HomeComponent } from './components/home/home.component';
import { MaterialExportsModule } from './components/material-exports/material-exports.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

@NgModule({
  declarations: [
    AppComponent,
    SignUpComponent,
    SignInComponent,
    HomeComponent
  ],
  imports: [
    HttpClientModule,
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    MaterialExportsModule,
    BrowserAnimationsModule
  ],
  providers: [
    { provide: Window, useValue: window },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptor,
      multi: true
   },
   { 
     provide: APP_INITIALIZER, 
     useFactory: (currentUserService: CurrentUserService) => () => currentUserService.loadCurrentUser(), 
     deps: [CurrentUserService], 
     multi: true 
   } ,
   AuthService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }