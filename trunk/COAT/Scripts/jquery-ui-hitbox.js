(function($,document){
		$.fn.hitbox = function(opt){
			var _default={
				hitbox:'div',
				head:'div[title]',
				item:'input[type="checkbox"]',
				selected:'input[type="checkbox"]:checked',
				closer:'.ui-dialog-titlebar-close',
				spliter:' '
			};	
			var _speed= 250;
			var _ctnClass = 'ui-dialog ui-widget ui-widget-content ui-corner-all';
			var _headClass = 'ui-datepicker-header ui-widget-header ui-helper-clearfix ui-corner-all';
			var _option = opt||_default;
			var _input = $(this);
			var _hitbox = $(this).next(_option.hitbox);

			_initialHead = function(){
				var head = _hitbox.find(_option.head);
				head.addClass(_headClass);
				var title = head.attr('title');
				head.prepend('<div class="ui-dialog-titlebar ui-widget-header ui-corner-all ui-helper-clearfix">'
							    +'<span class="ui-dialog-title" id="ui-dialog-title-dialog">'+title+'</span>'
								+'<a class="ui-dialog-titlebar-close ui-corner-all" role="button" href="#">'
									+'<span class="ui-icon ui-icon-closethick">close</span>'
								+'</a>'
							+'</div>'
							);	
			};
			
			_styleHitbox = function(){
				_hitbox.addClass(_ctnClass);
				_hitbox.hide();
			};
			
			_setItemChecked = function(){
				var _vals = _input.val().split(_option.spliter);
				_hitbox.find(_option.item).each(function(){
					if($.inArray($(this).val(),_vals)>=0){
						$(this).attr('checked', true);
					}else{
						$(this).attr('checked', false);
					}
				});
			};
			
			_setHitBoxPosition = function(){
				var _offset = _input.offset();
				_hitbox.css({ top:  _offset.top, left: _offset.left, position:'absolute' });
			};
			
			_showHitBox = function(){
				_setItemChecked();
				_setHitBoxPosition();
				_hitbox.show(_speed);	
				_hitbox.focus();
			};
			
			_attachShowEvent = function(){
				_input.focus(function(){
					_showHitBox();
				});
			}
			
			_setInputValue = function(){
					_input.val('');
					_hitbox.find(_option.selected).each(function(){
						var val = _input.val()+_option.spliter+$(this).val();
						_input.val(val);	
					});
			};
			
			_hideHitbox = function(){
					_setInputValue();
					_hitbox.hide(_speed);
			};
			
			_attachHideEvent = function(){
				_hitbox.find(_option.closer).click(function(){
					_hideHitbox();
				});
			};
			
			_prevnetPopEvent = function(){
				_input.click(function(event){
					event.stopPropagation();
				});
				_hitbox.click(function(event){
					event.stopPropagation();
				});
			};
			
			initialHitbox = function(){
				_prevnetPopEvent();
				_styleHitbox();
				_initialHead();
				_attachShowEvent();
				_attachHideEvent();
			}

			initialHitbox();
			$(document).click(_hideHitbox);
		}	
})(jQuery,document);	