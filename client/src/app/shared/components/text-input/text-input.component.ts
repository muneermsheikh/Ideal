import { Component, OnInit, ViewChild, ElementRef, Input, Self } from '@angular/core';
import { AsyncValidatorFn, ControlValueAccessor, NgControl } from '@angular/forms';
import { of, timer } from 'rxjs';
import { map, switchMap } from 'rxjs/operators';
import { AccountService } from 'src/app/account/account.service';
import { UsersService } from 'src/app/users/users.service';

@Component({
  selector: 'app-text-input',
  templateUrl: './text-input.component.html',
  styleUrls: ['./text-input.component.scss']
})
export class TextInputComponent implements OnInit, ControlValueAccessor {
  @ViewChild('input', { static: true }) input: ElementRef;
  @Input() type = 'text';
  @Input() label: string;
  @Input() id: string;
  
  constructor(@Self() public controlDir: NgControl, private acctService: AccountService, private service: UsersService) {
    this.controlDir.valueAccessor = this;
  }

  ngOnInit() {
    const control = this.controlDir.control;
    const validators = control.validator ? [control.validator] : [];
    const asyncValidators = control.asyncValidator ? [control.asyncValidator] : [];
    if (this.id === '') {
      this.id = this.label;
    }
    control.setValidators(validators);
    control.setAsyncValidators(asyncValidators);
    control.updateValueAndValidity();
  }

  onChange(event) { }

  onTouched() { }

  writeValue(obj: any): void {
    this.input.nativeElement.value = obj || '';
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }


  validateEmailNotTaken(): AsyncValidatorFn {
    return control => {
      return timer(500).pipe(
        switchMap(() => {
          if (!control.value)
          {
            return of(null);
          }
          return this.acctService.checkEmailExists(control.value).pipe(
            map(res => {
              return res ? {emailExists: true} : null;
            })
          );
        })
      );
    };
  }


  validatePPNumberNotTaken(): AsyncValidatorFn {
    return control => {
      return timer(500).pipe(
        switchMap(() => {
          if (!control.value)
          {
            return of(null);
          }
          return this.service.checkPPNoExists(control.value).pipe(
            map(res => {
              return res ? {ppNoExists: true} : null;
            })
          );
        })
      );
    };
  }

  validateAadharNumberNotTaken(): AsyncValidatorFn {
    return control => {
      return timer(500).pipe(
        switchMap(() => {
          if (!control.value)
          {
            return of(null);
          }
          return this.service.checkAadharNoExists(control.value).pipe(
            map(res => {
              return res ? {aadharNoExists: true} : null;
            })
          );
        })
      );
    };
  }

}
