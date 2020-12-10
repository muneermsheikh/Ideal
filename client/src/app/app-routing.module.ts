import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from './core/guards/auth.guard';
import { NotFoundComponent } from './core/not-found/not-found.component';
import { ServerErrorComponent } from './core/server-error/server-error.component';
import { TestErrorComponent } from './core/test-error/test-error.component';
import { EmailCreateComponent } from './email/email-create/email-create.component';
import { EmailComponent } from './email/email.component';
import { EmpCreateComponent } from './employees/emp-create/emp-create.component';
import { EmployeesComponent } from './employees/employees/employees.component';
import { HomeComponent } from './home/home.component';
import { OrderCreateComponent } from './orders/order-create/order-create.component';
import { OrderEditComponent } from './orders/order-edit/order-edit.component';
import { OrdersComponent } from './orders/orders.component';
import { ProfessionAddComponent } from './profession/profession-add/profession-add.component';
import { ProfessionDetailComponent } from './profession/profession-detail/profession-detail.component';
import { ProfessionComponent } from './profession/profession.component';
import { ReviewCreateComponent } from './reviews/review-create/review-create.component';
import { ReviewsComponent } from './reviews/reviews.component';
import { CandidateCreateComponent } from './users/candidate/candidate-create/candidate-create.component';
import { CandidateComponent } from './users/candidate/candidate.component';


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
  {path: 'empCreate', component: EmpCreateComponent, data: {breadcrumb: 'add an employee'}},
  {path: 'empEdit/:id', component: EmpCreateComponent, data: {breadcrumb: 'edit an employee'}},
  {path: 'candidate', component: CandidateComponent, data: {breadcrumb: 'Candidate list'}},
  {path: 'candidate-create', component: CandidateCreateComponent, data: {breadcrumb: 'add a candidate'}},
  {path: 'candidateEdit/:id', component: CandidateCreateComponent, data: {breadcrumb: 'edit candidate'}},
  {path: 'employees', component: EmployeesComponent, data: {breadcrumb: 'edit candidate'}},
  {path: 'users', loadChildren: () => import('./users/users.module')
    .then(mod => mod.UsersModule), data: {breadcrumb: {skip: true}} },
  {path: 'account', loadChildren: () => import('./account/account.module')
    .then(mod => mod.AccountModule), data: {breadcrumb: {skip: true}} },
  {path: 'enquiry', component: OrdersComponent, data: {breadcrumb: 'Enquiries'}},
  {path: 'enquiryCreate', component: OrderCreateComponent, data: {breadcrumb: 'Enquiry Create'}},
  {path: 'enquiryEdit/:id', component: OrderEditComponent, data: {breadcrumb: 'edit Enquiry'}},
  {path: 'review', component: ReviewsComponent, data: {breadcrumb: 'contract Reviews'}},
  {path: 'reviewCreate', component: ReviewCreateComponent, data: {breadcrumb: 'review create'}},
  {path: 'reviewEdit/:id', component: ReviewCreateComponent, data: {breadcrumb: 'edit Reviews'}},
  {path: 'email', component: EmailComponent, data: {breadcrumb: 'email'}},
  {path: 'emailCreate/:id', component: EmailCreateComponent, data: {breadcrumb: 'email edit'}},
  {path: '**', redirectTo: 'not-found', pathMatch: 'full'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
