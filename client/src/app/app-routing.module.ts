import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from './core/guards/auth.guard';
import { NotFoundComponent } from './core/not-found/not-found.component';
import { ServerErrorComponent } from './core/server-error/server-error.component';
import { TestErrorComponent } from './core/test-error/test-error.component';
import { EmpCreateComponent } from './employees/emp-create/emp-create.component';
import { EmployeesComponent } from './employees/employees/employees.component';
import { HomeComponent } from './home/home.component';
import { ProfessionAddComponent } from './profession/profession-add/profession-add.component';
import { ProfessionDetailComponent } from './profession/profession-detail/profession-detail.component';
import { ProfessionComponent } from './profession/profession.component';
import { CandidateAddComponent } from './users/candidate/candidate-add/candidate-add.component';
import { CandidateCreateComponent } from './users/candidate/candidate-create/candidate-create.component';
/*
import { CategoryDetailsComponent } from './shop/category-details/category-details.component';
import { ShopComponent } from './shop/shop.component';
*/

const routes: Routes = [
  {path: '', component: HomeComponent, data: {breadcrumb: 'home'}},
  {path: 'test-error', component: TestErrorComponent, data: {breadcrumb: 'test errors'}},
  {path: 'not-found', component: NotFoundComponent, data: {breadcrumb: 'not found error'}},
  {path: 'server-error', component: ServerErrorComponent, data: {breadcrumb: 'server error'}},
  {path: 'profession', component: ProfessionComponent, data: {breadcrumb: 'profession'}},
  {path: 'profession/:id', component: ProfessionDetailComponent, data: {breadcrumb: 'category detail'}},
  {path: 'shop', loadChildren: () => import('./shop/shop.module')
    .then(mod => mod.ShopModule), data: {breadcrumb: 'shop'} },
  {path: 'basket', loadChildren: () => import('./basket/basket.module')
    .then(mod => mod.BasketModule), data: {breadcrumb: 'basket'} },
  {path: 'checkout',
    canActivate: [AuthGuard],
    loadChildren: () => import('./checkout/checkout.module')
    .then(mod => mod.CheckoutModule), data: {breadcrumb: 'check out'} },
  {path: 'profession', loadChildren: () => import('./profession/profession.module')
    .then(mod => mod.ProfessionModule), data: {breadcrumb: {skip: true}} },
  {path: 'profession-add', component: ProfessionAddComponent, data: {breadcrumb: 'add a category'}},
  {path: 'candidate-add', component: CandidateAddComponent, data: {breadcrumb: 'add a candidate'}},
  {path: 'candidate-create', component: CandidateCreateComponent, data: {breadcrumb: 'add a candidate'}},
  {path: 'candidateEdit/:id', component: CandidateCreateComponent, data: {breadcrumb: 'edit candidate'}},
  {path: 'employees', component: EmployeesComponent, data: {breadcrumb: 'edit candidate'}},
  {path: 'empCreate', component: EmpCreateComponent, data: {breadcrumb: 'new employee'}},
  {path: 'users', loadChildren: () => import('./users/users.module')
    .then(mod => mod.UsersModule), data: {breadcrumb: {skip: true}} },
  {path: 'account', loadChildren: () => import('./account/account.module')
    .then(mod => mod.AccountModule), data: {breadcrumb: {skip: true}} },
  {path: '**', redirectTo: 'not-found', pathMatch: 'full'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
