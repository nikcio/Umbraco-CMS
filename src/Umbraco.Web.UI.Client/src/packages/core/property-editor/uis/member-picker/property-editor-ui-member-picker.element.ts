import { html, customElement, property, state } from '@umbraco-cms/backoffice/external/lit';
import type { UmbPropertyEditorUiElement } from '@umbraco-cms/backoffice/extension-registry';
import { UmbLitElement } from '@umbraco-cms/backoffice/lit-element';
import type { UmbPropertyEditorConfigCollection } from '@umbraco-cms/backoffice/property-editor';
import type { UmbInputMemberElement } from '@umbraco-cms/backoffice/member';

/**
 * @element umb-property-editor-ui-member-picker
 */
@customElement('umb-property-editor-ui-member-picker')
export class UmbPropertyEditorUIMemberPickerElement extends UmbLitElement implements UmbPropertyEditorUiElement {
	private _value: Array<string> = [];

	@property({ type: Array })
	public get value(): Array<string> {
		return this._value;
	}
	public set value(value: Array<string>) {
		this._value = Array.isArray(value) ? value : value ? [value] : [];
	}

	@property({ attribute: false })
	public set config(config: UmbPropertyEditorConfigCollection | undefined) {
		const validationLimit = config?.find((x) => x.alias === 'validationLimit');

		this._limitMin = (validationLimit?.value as any)?.min;
		this._limitMax = (validationLimit?.value as any)?.max;
	}

	@state()
	private _limitMin?: number;
	@state()
	private _limitMax?: number;

	private _onChange(event: CustomEvent) {
		console.log('event', event);
		this.value = (event.target as UmbInputMemberElement).selectedIds;
		console.log('this.value', this.value);
		this.dispatchEvent(new CustomEvent('property-value-change'));
	}

	// TODO: Implement mandatory?
	render() {
		return html`
			<umb-input-member
				@change=${this._onChange}
				.selectedIds=${this._value}
				.min=${this._limitMin ?? 0}
				.max=${this._limitMax ?? Infinity}
				>Add</umb-input-member
			>
		`;
	}
}

export default UmbPropertyEditorUIMemberPickerElement;

declare global {
	interface HTMLElementTagNameMap {
		'umb-property-editor-ui-member-picker': UmbPropertyEditorUIMemberPickerElement;
	}
}
