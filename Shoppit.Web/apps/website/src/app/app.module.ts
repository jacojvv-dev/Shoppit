import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { APP_CONFIG } from '@shoppit/shared/app-config';
import * as sharedState from '@shoppit/shared/state';
import { OAuthModule } from 'angular-oauth2-oidc';
import { environment } from '../environments/environment';
import { AppComponent } from './app.component';

@NgModule({
  declarations: [AppComponent],
  imports: [
    BrowserModule,
    NgbModule,
    HttpClientModule,
    RouterModule.forRoot([
      { path: '', pathMatch: 'full', redirectTo: 'products' },
      {
        path: 'products',
        loadChildren: () =>
          import('@shoppit/website/products').then(
            (module) => module.WebsiteProductsModule
          ),
      },
      {
        path: 'about',
        loadChildren: () =>
          import('@shoppit/website/about').then(
            (module) => module.WebsiteAboutModule
          ),
      },
      {
        path: 'cart',
        loadChildren: () =>
          import('@shoppit/website/cart').then(
            (module) => module.WebsiteCartModule
          ),
      },
    ]),
    OAuthModule.forRoot({
      resourceServer: {
        allowedUrls: [environment.apiRoute],
        sendAccessToken: true,
      },
    }),
    StoreModule.forRoot({}),
    EffectsModule.forRoot(),
    !environment.production ? StoreDevtoolsModule.instrument() : [],
    StoreModule.forFeature(sharedState.CART_FEATURE_KEY, sharedState.reducer),
    EffectsModule.forFeature([sharedState.CartEffects]),
  ],
  providers: [
    { provide: APP_CONFIG, useValue: environment },
    sharedState.CartFacade,
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
