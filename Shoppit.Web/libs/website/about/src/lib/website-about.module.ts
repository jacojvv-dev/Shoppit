import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { AboutIndexComponent } from './about-index/about-index.component';

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forChild([
      { path: '', pathMatch: 'full', component: AboutIndexComponent },
    ]),
  ],
  declarations: [AboutIndexComponent],
})
export class WebsiteAboutModule {}
