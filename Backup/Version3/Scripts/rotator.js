var InSite = {
    rotatorHelper: {
        isRotatorPaused: false,
        handlers: {
            onClientItemShown: function (sender, args) {
                InSite.rotatorHelper.selectTab(args.get_item().get_index());
            },
            onTabClick: function (sender,rotatorId) {
                var links = $get('slideshow-controls').getElementsByTagName("a");
                var index;

                for (var i = 0; i < links.length; i++) {
                    if (links[i] == sender) {
                        index = i;
                        break;
                    }
                }

                var rotator = $find(rotatorId);

                if (index < 5) {
                    rotator.set_currentItemIndex(index);
                    InSite.rotatorHelper.selectTab(index);
                }
                else if (InSite.rotatorHelper.isRotatorPaused) {
                    sender.innerHTML = 'Stop';
                    rotator.start();

                    InSite.rotatorHelper.isRotatorPaused = false;
                }
                else {
                    sender.innerHTML = 'Start';
                    rotator.stop();

                    InSite.rotatorHelper.isRotatorPaused = true;
                }
            }
        },
        selectTab: function (index) {
            var links = $get('slideshow-controls').getElementsByTagName("a");

            for (var i = 0; i < links.length; i++) {
                links[i].className = 'control';
            }

            links[index].className = 'control active';
        }
    }
};