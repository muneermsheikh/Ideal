import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CoreModule } from './core/core.module';
// import { ShopModule } from './shop/shop.module'; - not required fter lazy loading
import { HomeModule } from './home/home.module';
import { ErrorInterceptor } from './core/interceptors/error.interceptor';
import { NgxSpinnerModule } from 'ngx-spinner';
import { LoadingInterceptor } from './core/interceptors/loading.interceptors';
import { ProfessionModule } from './profession/profession.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { UsersModule } from './users/users.module';
import { EmployeesComponent } from './employees/employees/employees.component';
import { EmpCreateComponent } from './employees/emp-create/emp-create.component';
import { OrdersModule } from './orders/orders.module';
import { EmailModule } from './email/email.module';
import { ReviewsModule } from './reviews/reviews.module';
import { RightClickMenuComponent } from './shared/components/right-click-menu/right-click-menu.component';


@NgModule({
  declarations: [
    AppComponent,
    EmployeesComponent,
    EmpCreateComponent
    // , RightClickMenuComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    HttpClientModule,
    CoreModule,
    // ShopModule,    // taken out after lazy loading of shop and category details routes
    HomeModule,
    NgxSpinnerModule,
    ProfessionModule,
    ReactiveFormsModule,
    FormsModule,
    UsersModule,
    OrdersModule,
    EmailModule,
    ReviewsModule
    // , MatSliderModule

  ],
  providers: [
    {provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true},
    {provide: HTTP_INTERCEPTORS, useClass: LoadingInterceptor, multi: true}
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
