import { NgModule } from '@angular/core';
import { CommonModule, FormStyle } from '@angular/common';
import { RouterModule, Route } from '@angular/router';
import { CartIndexComponent } from './containers/cart-index/cart-index.component';
import { FormsModule } from '@angular/forms';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    RouterModule.forChild([
      { path: '', pathMatch: 'full', component: CartIndexComponent },
    ]),
  ],
  declarations: [CartIndexComponent],
})
export class WebsiteCartModule {}
