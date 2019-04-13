import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { SignInComponent } from './components/sign-in/sign-in.component';
import { SignUpComponent } from './components/sign-up/sign-up.component';
import { NavigationComponent } from './components/navigation/navigation.component';
import { LotComponent } from './components/lot/lot.component';
import { LotListComponent } from './components/lot-list/lot-list.component';
import { AddLotComponent } from './components/add-lot/add-lot.component';
import { EditLotComponent } from './components/edit-lot/edit-lot.component';
import { UserListComponent } from './components/user-list/user-list.component';
import { UserComponent } from './components/user/user.component';
import { EditUserComponent } from './components/edit-user/edit-user.component';
import { AuthGuard } from './guards/auth.guard';
import { AdminGuard } from './guards/admin.guard';
import { SellerGuard } from './guards/seller.guard';

const routes: Routes = [
  { path: 'sign-in', component: SignInComponent, },
  { path: 'sign-up', component: SignUpComponent },
  { path: 'lot/:id', component: LotComponent },
  { path: 'create-lot', component: AddLotComponent, canActivate: [SellerGuard], canActivateChild: [SellerGuard] },
  { path: 'edit-lot/:id', component: EditLotComponent, canActivate: [SellerGuard], canActivateChild: [SellerGuard] },
  { path: 'users', component: UserListComponent, canActivate: [AdminGuard], canActivateChild: [AdminGuard] },
  { path: 'edit-user/:id', component: EditUserComponent, canActivate: [AuthGuard], canActivateChild: [AuthGuard] },
  { path: '', component: LotListComponent, pathMatch: 'full' },
  { path: 'user/:id', component: UserComponent, canActivate: [AuthGuard], canActivateChild: [AuthGuard] },
  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
