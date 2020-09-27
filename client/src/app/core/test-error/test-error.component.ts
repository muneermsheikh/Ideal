import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-test-error',
  templateUrl: './test-error.component.html',
  styleUrls: ['./test-error.component.scss']
})
export class TestErrorComponent implements OnInit {
  baseUrl = environment.apiUrl;
  validationErrors: any;

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
  }

  get404Error(){
    return this.http.get(this.baseUrl + 'category/9999').subscribe(response => {
      console.log(response);
    }, error => {
      console.log(error);
    });
  }

  get400Error(){
    return this.http.get(this.baseUrl + 'buggy/badrequest').subscribe(response => {
      console.log(response);
    }, error => {
      console.log(error);
    });
  }

  get500Error(){
    return this.http.get(this.baseUrl + 'buggy/servererror').subscribe(response => {
      console.log(response);
    }, error => {
      console.log(error);
    });
  }

  get400validationError(){
    return this.http.get(this.baseUrl + 'category/idinwords').subscribe(response => {
      console.log(response);
    }, error => {
      console.log(error);
      this.validationErrors = error.errors;
    });
  }
}
