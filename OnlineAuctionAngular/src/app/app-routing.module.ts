import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { SignInComponent } from './components/sign-in/sign-in.component';
import { SignUpComponent } from './components/sign-up/sign-up.component';
import { NavigationComponent } from './components/home/navigation.component';
import { LotComponent } from './components/lot/lot.component';
import { LotListComponent } from './components/lot-list/lot-list.component';
import { AddLotComponent } from './components/add-lot/add-lot.component';
import { EditLotComponent } from './components/edit-lot/edit-lot.component';

const routes: Routes = [
  { path: 'sign-in', component: SignInComponent },
  { path: 'sign-up', component: SignUpComponent },
  { path: 'lot/:id', component: LotComponent },
  { path: 'create-lot', component: AddLotComponent },
  { path: 'edit-lot/:id', component: EditLotComponent },
  { path: '', component: LotListComponent, pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
