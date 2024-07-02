import { UmbInputEyeDropperElement } from './input-eye-dropper.element.js';
import { expect, fixture, html } from '@open-wc/testing';
import { type UmbTestRunnerWindow, defaultA11yConfig } from '@umbraco-cms/internal/test-utils';
describe('UmbInputEyeDropperElement', () => {
	let element: UmbInputEyeDropperElement;

	beforeEach(async () => {
		element = await fixture(html` <umb-input-eye-dropper></umb-input-eye-dropper> `);
	});

	it('is defined with its own instance', () => {
		expect(element).to.be.instanceOf(UmbInputEyeDropperElement);
	});

	if ((window as UmbTestRunnerWindow).__UMBRACO_TEST_RUN_A11Y_TEST) {
		it('passes the a11y audit', async () => {
			await expect(element).shadowDom.to.be.accessible(defaultA11yConfig);
		});
	}
});
