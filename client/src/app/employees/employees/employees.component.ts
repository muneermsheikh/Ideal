import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { IEmployee } from 'src/app/shared/models/employee';
import { EmployeeParams } from 'src/app/shared/models/employeeParams';
import { EmployeesService } from '../employees.service';

@Component({
  selector: 'app-employees',
  templateUrl: './employees.component.html',
  styleUrls: ['./employees.component.scss']
})

export class EmployeesComponent implements OnInit {
  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  employees: IEmployee[];
  form: FormGroup;
  errors: string[];

  empParams = new EmployeeParams();
  totalCount: number;
  sortSelected = 'asc';
  sortOptions = [
    { name: 'Ascending by category', value: 'categoryNameasc' },
    { name: 'Descending by category', value: 'categoryNameDesc' },
    { name: 'Ascending by City Name', value: 'cityNameTypeAsc' },
    { name: 'Descending by City Name', value: 'cityNameDesc' }
  ];

  constructor(private service: EmployeesService, private fb: FormBuilder,
              private router: Router) { }

  ngOnInit(): void {

    this.getEmployees();
    this.createForm();
  }

  editButtonClick(employeeId: number): void {
    this.router.navigate(['/empEdit', employeeId]);
  }

  getEmployees(useCache = false): void {

      this.service.getEmployees(useCache).subscribe(response => {
        this.employees = response.data;
        console.log(this.employees);
        this.totalCount = response.count;
        this.empParams.pageNumber = response.pageIndex;
        this.empParams.pageSize = response.pageSize;
    }, error => {
      console.log(error);
    });
  }

  createForm(): void {
    this.form = this.fb.group({
      id: [null],
      firstName: [null],
      secondName: [null],
      familyName: [null],
      gender: [null],
      knownAs: [null],
      fullName: [null],
      dOB: [null],
      pPNo: [null],
      mobile: [null],
      email: [null],
      address: [null],
      eCNR: [null]
    });
  }

  onPageChanged(event: any): void {
    const params = this.service.getEmpParams();
    if (params.pageNumber !== event) {
      params.pageNumber = event;
      this.service.setEmpParams(params);
      this.getEmployees(true);
    }
  }

  onSearch(): void{

    const params = this.service.getEmpParams();
    params.search = this.searchTerm.nativeElement.value;
    params.pageNumber = 1;
    this.service.setEmpParams(params);

    this.getEmployees();
  }


  onReset(): any {
    this.searchTerm.nativeElement.value = '';
    this.empParams = new EmployeeParams();
    this.getEmployees();
  }

  onSortSelected(sort: string): any {
    const params = this.service.getEmpParams();
    params.sort = sort;
    this.service.setEmpParams(params);
    this.getEmployees();
  }

}
